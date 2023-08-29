import os, sys, pika
from uuid import uuid4
from wand.image import Image
import psycopg2

# Identify the hostname of the RabbitMQ instance
mq_host = os.environ.get('SHOP_MESSAGING_HOST')
images_root = os.environ.get('SHOP_IMAGE_CACHE')

def resize(img_name):
    file_id = img_name.split('.')
    guid = file_id[0]
    path = os.path.join(images_root, img_name)

    with Image(filename=path) as img:
        img.format = 'png'
        img.auto_orient()

        thumbnail_path = os.path.join(images_root, "{0}-thumbnail.png".format(guid))
        thumbnail = scale_crop(img, 225, 225)
        thumbnail.save(filename=thumbnail_path)

        carousel_path = os.path.join(images_root, "{0}-carousel.png".format(guid))
        carousel = scale_crop(img, 1920, 1080)
        carousel.save(filename=carousel_path)

    upload(thumbnail_path, guid, "thumbnail")
    upload(carousel_path, guid, "carousel")

    os.remove(thumbnail_path)
    os.remove(carousel_path)
    os.remove(path)

def scale_crop(img, width, height):
    cln = img.clone()
    old_size = cln.size
    new_size = scale(old_size[0], old_size[1], width, height)
    cln.resize(int(new_size[0]), int(new_size[1]))
    cln.crop(int((new_size[0] - width) / 2), int((new_size[1] - height) / 2), width=width, height=height)
    return cln

def scale(w_0, h_0, w_1, h_1):
    scale_a = w_1 / w_0
    scale_b = h_1 / h_0
    if h_0 * scale_a < h_1:
        return (w_0 * scale_b, h_0 * scale_b)
    else:
        return (w_0 * scale_a, h_0 * scale_a)

def upload(img_path, prod_guid, img_type):
    with open(img_path, 'rb') as f:
        data = f.read()
    length = os.path.getsize(img_path)
    img_guid = str(uuid4())
    conn = psycopg2.connect(database='shop_db',
                            host='shopdb',
                            user='postgres',
                            password='app_password'
                            )
    cursor = conn.cursor()
    cursor.execute("INSERT INTO \"Images\" (\"Id\", \"Data\", \"Length\") VALUES (%s, %s, %s)", (img_guid, data, length))

    if img_type == "thumbnail":
        cursor.execute("UPDATE \"ProductImages\" SET \"ThumbnailId\" = %s WHERE \"Id\" = %s", (img_guid, prod_guid))
    elif img_type == "carousel":
        cursor.execute("UPDATE \"ProductImages\" SET \"ImageId\" = %s WHERE \"Id\" = %s", (img_guid, prod_guid))

    conn.commit()
    cursor.close()
    conn.close()

def callback(ch, method, properties, body):
    try:
        img = body.decode('utf-8')
        resize(img)
        ch.basic_ack(delivery_tag = method.delivery_tag)
        print('Converted image {0}'.format(img))
    except Exception as ex:
        print('Failed to process image: {0}'.format(ex))

def main():
    print("Connecting to {0}.".format(mq_host))
    connection = pika.BlockingConnection(pika.ConnectionParameters(host=mq_host))
    channel = connection.channel()

    channel.queue_declare(queue='resize')

    channel.basic_consume(queue='resize', on_message_callback=callback, auto_ack=False)

    print(' [*] Waiting for messages. To exit press CTRL+C')
    channel.start_consuming()

if __name__ == '__main__':
    try:
        main()
    except KeyboardInterrupt:
        print('Interrupted')
        try:
            sys.exit(0)
        except SystemExit:
            os._exit(0)
FROM python:3
RUN apt-get install libmagickwand-dev
WORKDIR /usr/src/app
COPY requirements.txt ./
RUN pip install --trusted-host pypi.org --trusted-host pypi.python.org --trusted-host=files.pythonhosted.org --no-cache-dir -r /usr/src/app/requirements.txt
COPY . .
CMD [ "python", "-u", "shop_img_worker.py" ]
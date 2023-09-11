#!/usr/bin/bash
python3 -m venv env;
source env/bin/activate;
pip3 install --trusted-host pypi.org --trusted-host pypi.python.org --trusted-host=files.pythonhosted.org --no-cache-dir --pre sqlacodegen;
pip install --trusted-host pypi.org --trusted-host pypi.python.org --trusted-host=files.pythonhosted.org --no-cache-dir psycopg2-binary;

sleep(10)
sqlacodegen postgresql://postgres:app_password@localhost;
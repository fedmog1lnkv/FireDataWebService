import uvicorn
from flask import Flask
from src import startup, ENV_CONF

if __name__ == "__main__":
    app = Flask(__name__)
    startup(app)
    uvicorn.run(app, host=ENV_CONF.IPADDR, port=ENV_CONF.PORT)
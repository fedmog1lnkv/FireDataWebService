from flask import Flask
from src import startup
from src.config.EnvConfig import ENV_CONF

if __name__ == "__main__":
    app = Flask(__name__)
    startup(app)
    app.run(host=ENV_CONF.IPADDR, port=ENV_CONF.PORT)

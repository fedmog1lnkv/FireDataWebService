from dotenv import load_dotenv
import os
from src.config.EnvConfig import EnvConfig
from src.routers import index_router

ENV_CONF = EnvConfig(IPADDR="", PORT=0)


def startup(app):
    config_env()
    include_routers(app)


def config_env():
    load_dotenv()

    global ENV_CONF
    ENV_CONF.IPADDR = os.getenv("IPADDR")
    ENV_CONF.PORT = int(os.getenv("PORT"))


def include_routers(app):
    app.register_blueprint(index_router)

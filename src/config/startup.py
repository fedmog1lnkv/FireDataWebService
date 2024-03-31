from dotenv import load_dotenv
import os

from src.config.EnvConfig import EnvConfig

ENV_CONF: EnvConfig


def startup(app):
    config_env()


def config_env():
    load_dotenv()

    global ENV_CONF
    ENV_CONF.IPADDR = os.getenv("IPADDR")
    ENV_CONF.PORT = os.getenv("PORT")

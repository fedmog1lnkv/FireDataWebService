import os
from dataclasses import dataclass

from dotenv import load_dotenv


@dataclass
class EnvConfig:
    IPADDR: str = ""
    PORT: int = 0


def config_env():
    load_dotenv()

    ENV_CONF.IPADDR = os.getenv("IPADDR")
    ENV_CONF.PORT = int(os.getenv("PORT"))


ENV_CONF = EnvConfig()

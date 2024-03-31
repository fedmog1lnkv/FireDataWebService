from dataclasses import dataclass

@dataclass
class EnvConfig:
    IPADDR: str
    PORT: int
from flask import Blueprint

index_router = Blueprint('index', __name__)


@index_router.route('/')
def index():
    return 'Hello World'
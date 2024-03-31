from flask import Blueprint, request

index_router = Blueprint('index', __name__)


@index_router.route('/')
def index():
    return "Hello world!"

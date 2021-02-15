import json

# Place this code in a Lambda function to get the Public IP of the current user
# API Gateway also needs to be configured

def lambda_handler(event, context):
    return {
        'statusCode': 200,
        'body': event["requestContext"]["identity"]["sourceIp"]
    }

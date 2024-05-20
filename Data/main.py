from selenium import webdriver
from selenium.webdriver.chrome.service import Service
from selenium.webdriver.common.by import By
from bs4 import BeautifulSoup
import time
import random
import base64
import os
import requests
import google.generativeai as genai
import PIL.Image
import re
from pymongo import MongoClient
import json
genai.configure(api_key="AIzaSyBsgCgtPDG9Jwo-lmDpS8KXX9obpb0x3Fk")

def sort_filenames_by_number(filenames):
    """Sorts a list of filenames containing numbers by the numerical value."""

    def extract_number(filename):
        """Extracts the numerical part from a filename and converts it to an integer."""
        match = re.search(r'\d+', filename)  # Find any sequence of digits
        return int(match.group()) if match else 0

    return sorted(filenames, key=extract_number)

def capitalize_keys(d):
  for key in list(d.keys()):
    new_key = key.capitalize()
    d[new_key] = d.pop(key)  

model = genai.GenerativeModel("gemini-pro-vision")

prompt = """
identifique a raça, cor, características e todas as outras informações que puder extrair sobre o cachorro na foto.
o retorno deve ser no seguinte formato:
{
    "Valid": true,
    "Raca": "apenas o nome da raça",
    "Cor": "apenas a cor principal",
    "Pelagem": "",
    "Olhos": "",
    "Focinho": "",
    "Tamanho": "",
    "Idade": "",
    "OutrasInformacoes": "outras informações sobre a foto"
}
se a imagem não conter um cachorro, ou o cachorro da foto for um desenho, retorne o seguinte json:
{
    "Valid": false
}
"""

abrigo = 'resgatadosestancia'
folder_path = f'/Users/teteusantosgd/Projects/BuscarMeuPet/WebApp/ClientApp/src/assets/Fotos/{abrigo}'

file_list = os.listdir(folder_path)
file_list = [f for f in file_list if f.endswith('.jpg')]
file_list = sort_filenames_by_number(file_list)

client = MongoClient("mongodb://localhost:27017")
database = client["buscarmeupet"]

for image in file_list:
    img = PIL.Image.open(f'{folder_path}/{image}')

    response = model.generate_content([prompt, img], stream=True)
    response.resolve()

    response = response.text.replace('```json', '').replace('```', '')

    pet = json.loads(response)
    capitalize_keys(pet)
    if pet['Valid'] == False:
        continue
    pet['Image'] = f'{abrigo}/{image}'
    pet['Abrigo'] = abrigo
    pet['Tipo'] = 'cachorro'
    pet['OutrasInformacoes'] = pet['Outrasinformacoes']
    cor = pet['Cor']
    pet['Raca'] = pet['Raca'].replace(cor, '').strip()
    pet.pop('Outrasinformacoes')
    pet.pop('Valid')
    print('Pet:', pet)
    
    exists = database.Pets.find_one({ 'image': pet['Image'] })
    if exists:
        print('Pet já cadastrado')
        continue
    database.Pets.insert_one(pet)
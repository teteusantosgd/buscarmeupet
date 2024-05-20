from selenium import webdriver
from selenium.webdriver.chrome.service import Service
from selenium.webdriver.common.by import By
from bs4 import BeautifulSoup
import time
import random
import base64
import os
import requests

def scrape_instagram_posts(url, login_url):
    """Scrapes posts from an Instagram page."""
    driver = webdriver.Chrome()
    driver.get(login_url)
    driver.execute_script(f"window.location.href = '{url}';")
    time.sleep(5)
    prev_height = 0

    posts = []

    while True:
        driver.execute_script("window.scrollTo(0, document.body.scrollHeight);")
        time.sleep(random.uniform(3, 5))
        if driver.execute_script("return document.body.scrollHeight") == prev_height:
            break
        prev_height = driver.execute_script("return document.body.scrollHeight")

        soup = BeautifulSoup(driver.page_source, 'html.parser')
        images = soup.find_all('img')

        for image in images:
            image_url = image['src']
            if image_url not in posts:
                posts.append(image_url)

    driver.quit()
    return posts

abrigos = [
    'sosanimaiscanudos'
]

if __name__ == "__main__":
    for abrigo in abrigos:
        page_name = abrigo
        instagram_page_url = f"https://www.instagram.com/{page_name}/"
        login_url = 'https://www.instagram.com/_n/web_emaillogin?uid=21qpund&token=Dcf3yB&auto_send=0'

        os.makedirs(f'fotos/{page_name}', exist_ok=True) 

        all_posts = scrape_instagram_posts(instagram_page_url, login_url)
        
        for i, url in enumerate(all_posts):
            if url.startswith('http'):
                response = requests.get(url)
                with open(f"fotos/{page_name}/image_{i+1}.jpg", "wb") as f:
                    f.write(response.content)
                time.sleep(random.uniform(1, 2))
            
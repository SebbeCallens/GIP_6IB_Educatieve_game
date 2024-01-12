import requests
import os

#script om alle pokemons to downloaden
#gemaakt door Cédric Dewettinck (en ChatGPT)
output_directory = "PokemonPuzzels"
os.makedirs(output_directory, exist_ok=True)

# Iterate over the range of numbers from 1 to 1200
for number in range(1, 1011):
    # Format the number with leading zeros
    formatted_number = f"{number:03}"

    # Construct the URL for the Pokemon image
    url = f"https://assets.pokemon.com/assets/cms2/img/pokedex/full/{formatted_number}.png"

    # Send a GET request to the URL
    response = requests.get(url)

    # Check if the request was successful (status code 200)
    if response.status_code == 200:
        # Save the image to the output directory
        with open(os.path.join(output_directory, f"{formatted_number}.png"), "wb") as file:
            file.write(response.content)
        print(f"Downloaded: {formatted_number}.png")
    else:
        print(f"Failed to download: {formatted_number}.png")

print("Download completed.")

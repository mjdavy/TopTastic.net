# TopTastic.net
Show and play the top 40 music videos from the UK

Now uses EchoNest for artist bios and other metadata (tbd). 

* YouTube api key is loaded from client_secrets.json in the assets folder. You will need to create this file and add your api key
* EchoNest api key is loaded from echonest_secrets.xml in the assets folder. You will need to create this file and add your api key

# Setup 
In the Assets folder create two files
* Create echonest_secrets.xml (echonest API key) https://developer.echonest.com/ this is an xml file with the format 
<EchoNestApiKey>-- your api key --</EchoNestApiKey>

* Create client_secrets.json (google oAuth2 key) (https://console.developers.google.com/apis)

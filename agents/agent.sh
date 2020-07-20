#!/usr/bin/env bash

ROOT_URL='http://localhost';
HEARTHBEAT_URL=$ROOT_URL+"/api/hearthbeat";
DOWNLOAD_URL=$ROOT_URL+"/api/config";
DOWNLOAD_FOLDER="$HOME";
CONFIG_NAME='config';
CONFIG_EXTENSION='zip';
CONFIG_SHA1='';
IP=$(hostname -I);


hearthbeat(){
    # ping server on url 
    curl -H 'Content-Type:application/json' $HEARTHBEAT_URL -d "{'ip': '$IP'}";
}

zip_config_files(){
    #  gets config files and compresses it in $HOME directory
    zip "$HOME"/$CONFIG_NAME.$CONFIG_EXTENSION -r /etc/nginx/ /etc/apache2/*.conf /etc/httpd/*.conf /etc/httpd/conf/*.conf /etc/http /etc/ssh/ -x /etc/ssh/*key*
}

update_config_sha1(){
    # updates sha1 of compressed configs 
    CONFIG_SHA1=$(sha1sum "$DOWNLOAD_FOLDER"/$CONFIG_NAME.$CONFIG_EXTENSION | awk '{ print $1 }');
}

check_for_new_config(){
    # checks if get_config_sha1 is equal to currently used config on server
    # if [[ $(curl -s -o /dev/null -w "%{http_code}" newconfig.com) == 200 ]]; then
    #   get_and_setup_config;   
    # endif
    return;
}

get_and_setup_config(){
    # downloads a new config, uncompresses and moves to needed directories
    # curl $DOWNLOAD_URL -L -o $DOWNLOAD_FOLDER/$CONFIG_NAME.$CONFIG_EXTENSION;
    # zip -e
    # mv -r nginx /etc
    # mv -r apache /etc
    return;
}

restart_services(){
    # restart nginx and apache
    sudo systemctl restart nginx.service
    sudo systemctl restart apache2.service
    sudo systemctl restart sshd.service
    sudo systemctl restart ssh.service
}

help(){
    # print help
    echo -e "\$\$Simple configuration manager shell agent\$\$"
    echo -e "desc: used as cronjob"
    echo -e "flags:\n\t-hb/--hearthbeat - sends hearthbeat ping to server (run every 10 seconds)"
    echo -e "\t-c/--newconfig - check if there is missmatch/outdated config on server, downloads and moves it, and  restarts services (run every 60s)"
}

case $1 in
    -hb|--heathbeat)
        echo 'pinging server..'
        #hearthbeat;
        ;;
    -c|--newconfig)
        echo 'checking new config..'
        #check_for_new_config;
        ;;
    *)
        help;
        ;;
esac

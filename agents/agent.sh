#!/usr/bin/env bash

ROOT_URL='http://localhost';
HEARTHBEAT_URL=$ROOT_URL"/api/hearthbeat";
DOWNLOAD_URL=$ROOT_URL"/api/config";
CHECK_CONFIG_URL=$ROOT_URL"/api/check";
DOWNLOAD_FOLDER="$HOME";
CONFIG_NAME='config';
CONFIG_EXTENSION='zip';
CONFIG_SHA1='.';
SERVER_NAME='linux1';


hearthbeat(){
    curl -H 'Content-Type:application/json' $HEARTHBEAT_URL -d "{'serverName': '$SERVER_NAME'}";
}

zip_config_files(){
    zip "$HOME"/$CONFIG_NAME.$CONFIG_EXTENSION -r /etc/nginx/ /etc/apache2/*.conf /etc/httpd/*.conf /etc/httpd/conf/*.conf /etc/http /etc/ssh/ -x /etc/ssh/*key*
}

update_config_sha1(){
    CONFIG_SHA1=$(sha1sum "$DOWNLOAD_FOLDER"/$CONFIG_NAME.$CONFIG_EXTENSION | awk '{ print $1 }');
}

check_for_new_config(){
    if [[ $(curl -sb -H "Accept: application/json" $CHECK_CONFIG_URL -d "{'sha1': '$CONFIG_SHA1'}") == *"false"*  ]]; then 
        get_and_setup_config;
        restart_services;
    fi
}

get_and_setup_config(){
    curl $DOWNLOAD_URL -L -o $DOWNLOAD_FOLDER/$CONFIG_NAME.$CONFIG_EXTENSION;
    unzip $DOWNLOAD_FOLDER/$CONFIG_NAME.$CONFIG_EXTENSION
    mv -r $DOWNLOAD_FOLDER/nginx /etc
    mv -r $DOWNLOAD_FOLDER/apache /etc
    mv -r $DOWNLOAD_FOLDER/ssh /etc
    mv -r $DOWNLOAD_FOLDER/httpd /etc
}

restart_services(){
    # restart nginx and apache
    sudo systemctl restart nginx.service
    sudo systemctl restart apache2.service
    sudo systemctl restart sshd.service
    sudo systemctl restart ssh.service
}

help(){
    echo -e "\$\$Simple configuration manager shell agent\$\$"
    echo -e "desc: used as cronjob"
    echo -e "flags:\n\t-hb/--hearthbeat - sends hearthbeat ping to server (run every 10 seconds)"
    echo -e "\t-c/--newconfig - check if there is missmatch/outdated config on server, downloads and moves it, and  restarts services (run every 60s)"
}

case $1 in
    -hb|--heathbeat)
        hearthbeat;
        ;;
    -c|--newconfig)
        check_for_new_config;
        ;;
    *)
        help;
        ;;
esac

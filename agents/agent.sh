ROOT_URL = ""
HB_URL = $ROOT_URL + ""
IP = $(hostname -I)


hearthbeat(){
    # ping server on url 
    echo "Pinging server.."
}

zip_config_files(){
    #  gets config files and compresses it in $HOME directory
    echo "Get config files.."
    echo "Compress"
}

get_config_sha1(){
    # returns sha1 of compressed configs 
    echo "Pinging server.."
}

check_for_new_config(){
    # checks if get_config_sha1 is equal to currently used config on server
    echo "Checking config.."    
}

get_and_setup_config(){
    # downloads a new config, uncompresses and moves to needed directories
    echo "Downloading new config.."
}

restart_services(){
    # restart nginx and apache
    sudo systemctl restart nginx.service
    sudo systemctl restart apache2.service
}

help(){
    # print help
    echo "Printing help.."
}



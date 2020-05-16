package com.mskalnik.simpleconfigurationmanager.model

import java.util.*

class Server(name: String, online: Boolean, ip: String, os: String, config: String, lastOnline: Date?) {
    val name = name
    val online = online
    val ip = ip
    val os = os
    val config = config
    val lastOnline = lastOnline

    companion object {
        fun getServerList(numContacts: Int): ArrayList<Server> {
            var id = 0
            val contacts = ArrayList<Server>()
            for (i in 1..numContacts) {
                contacts.add(
                    Server(
                        "Server " + ++id,
                        id % 2 == 0,
                        "10.0.0.$id",
                        "Linux",
                        "config-no-$id",
                        Date(20190101)
                    )
                )
            }
            return contacts
        }
    }
}
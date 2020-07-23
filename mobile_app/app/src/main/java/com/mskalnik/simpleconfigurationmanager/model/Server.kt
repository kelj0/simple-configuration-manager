package com.mskalnik.simpleconfigurationmanager.model

import android.service.autofill.Dataset
import com.google.gson.GsonBuilder
import com.google.gson.reflect.TypeToken
import java.lang.reflect.Type


class Server(
    val idServer: String,
    val serverName: String,
    val ipAddress: String,
    val isOnline: Boolean,
    val operatingSystemId: Int,
    val deletedL: Boolean,
    val config: String
) {
    companion object {
        private const val SERVER_NAME = "http://10.0.2.2:3000"

        fun getServers(): List<Server> {
            val json = Util.fetchJson("$SERVER_NAME/servers")
            val dataType: Type = object : TypeToken<Collection<Server?>?>() {}.type

            return GsonBuilder()
                .create()
                .fromJson<List<Server>>(json, dataType)
        }

//        fun getServerList(numContacts: Int): ArrayList<Server> {
//            var id = 0
//            val contacts = ArrayList<Server>()
//            for (i in 1..numContacts) {
//                contacts.add(
//                    Server(
//                        "Server " + ++id,
//                        id % 2 == 0,
//                        "10.0.0.$id",
//                        "Linux",
//                        "config-no-$id",
//                        Date(20190101)
//                    )
//                )
//            }
//            return contacts
//        }
    }
}
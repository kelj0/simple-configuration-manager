package com.mskalnik.simpleconfigurationmanager.model

import com.google.gson.GsonBuilder
import com.google.gson.reflect.TypeToken
import java.lang.reflect.Type


class Server(
    val serverName: String,
    val ipAddress: String,
    val isOnline: Boolean,
    val operatingSystem: String
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
    }
}
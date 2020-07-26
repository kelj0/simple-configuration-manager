package com.mskalnik.simpleconfigurationmanager.controller

import android.os.StrictMode
import com.google.gson.Gson
import com.google.gson.GsonBuilder
import com.google.gson.reflect.TypeToken
import com.mskalnik.simpleconfigurationmanager.model.Server
import com.mskalnik.simpleconfigurationmanager.model.User
import com.mskalnik.simpleconfigurationmanager.model.Util
import okhttp3.MediaType
import okhttp3.MediaType.Companion.toMediaType
import okhttp3.OkHttpClient
import okhttp3.Request
import okhttp3.RequestBody.Companion.toRequestBody
import java.lang.reflect.Type

class ApiController {
    companion object {
        private const val SERVER_NAME           = "http://invent.hr:5000"
        private const val CREATE_USER           = "$SERVER_NAME/api/SCM/User/CreateUser"
        private const val GET_SERVER_BY_USER    = "$SERVER_NAME/api/SCM/Server/GetByUser/1"
        private const val REMOVE_SERVER         = "$SERVER_NAME/api/SCM/Server/RemoveConfiguration"

        private val JSON: MediaType = "application/json; charset=utf-8".toMediaType()

        fun create(user: User): String {
            startNewThread()
            val requestBody = Gson()
                .toJson(user)
                .toRequestBody(JSON)
            val request = Request.Builder()
                .url(CREATE_USER)
                .post(requestBody)
                .build();

            OkHttpClient().newCall(request).execute().use { response -> return response.body!!.string() }
        }

        fun getServers(): List<Server> {
            val json = Util.fetchJson(GET_SERVER_BY_USER)
            val dataType: Type = object : TypeToken<Collection<Server?>?>() {}.type

            return GsonBuilder()
                .create()
                .fromJson<List<Server>>(json, dataType)
        }

        private fun startNewThread() {
            val policy = StrictMode
                .ThreadPolicy
                .Builder()
                .permitAll()
                .build()

            StrictMode.setThreadPolicy(policy)
        }
    }
}
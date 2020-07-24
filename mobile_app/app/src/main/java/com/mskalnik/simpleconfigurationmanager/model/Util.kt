package com.mskalnik.simpleconfigurationmanager.model

import android.content.Context
import android.widget.Toast
import okhttp3.OkHttpClient
import okhttp3.Request

class Util {
    companion object {
        fun fetchJson(url: String): String? {
            val client = OkHttpClient()
            val request = Request
                .Builder()
                .url(url)
                .build()

            client.newCall(request).execute().use { response -> return response.body!!.string() }
        }

        fun showToast(context: Context, text: String) {
            Toast.makeText(context, text, Toast.LENGTH_SHORT).show()
        }
    }
}
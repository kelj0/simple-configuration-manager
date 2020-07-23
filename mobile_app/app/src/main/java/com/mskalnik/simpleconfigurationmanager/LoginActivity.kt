package com.mskalnik.simpleconfigurationmanager

import android.content.Intent
import android.os.Bundle
import android.os.StrictMode
import android.widget.Toast
import com.mskalnik.simpleconfigurationmanager.model.Server
import com.mskalnik.simpleconfigurationmanager.model.Util
import kotlinx.android.synthetic.main.activity_login.*
import java.net.URL


class LoginActivity : BaseActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_login)

        supportActionBar?.setHomeButtonEnabled(true)

        btnLogin.setOnClickListener {
            when {
                checkLoginCredentials() -> startActivity(Intent(this, ServerListActivity::class.java))
                else -> Util.showToast(this, getString(R.string.wrong_email_password))
            }
        }
    }

    private fun checkLoginCredentials(): Boolean {
        val policy =
            StrictMode.ThreadPolicy.Builder().permitAll().build()

        StrictMode.setThreadPolicy(policy)

        // 10.0.2.2 because 127.0.0.1 is emulator
        // var text = URL("http://10.0.2.2:3000/users/2").readText()
        return etLoginEmail.text.toString() == "mail" && etLoginPassword.text.toString() == "pass"
    }
}

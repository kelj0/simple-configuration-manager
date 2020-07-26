package com.mskalnik.simpleconfigurationmanager

import android.content.Intent
import android.os.Bundle
import android.os.StrictMode
import com.mskalnik.simpleconfigurationmanager.model.Util
import kotlinx.android.synthetic.main.activity_login.*


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
        return etLoginEmail.text.toString() == "mail" && etLoginPassword.text.toString() == "pass"
    }
}

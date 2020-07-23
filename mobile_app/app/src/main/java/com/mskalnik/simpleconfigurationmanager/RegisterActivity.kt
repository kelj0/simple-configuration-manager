package com.mskalnik.simpleconfigurationmanager

import android.content.Intent
import android.os.Bundle
import kotlinx.android.synthetic.main.activity_register.*

class RegisterActivity : BaseActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_register)

        supportActionBar?.setHomeButtonEnabled(true)

        btnRegister.setOnClickListener {
            startActivity(Intent(this, WelcomeActivity::class.java))
        }
    }
}

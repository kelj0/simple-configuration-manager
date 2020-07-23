package com.mskalnik.simpleconfigurationmanager

import android.content.Intent
import android.os.Bundle
import android.widget.EditText
import com.mskalnik.simpleconfigurationmanager.model.Util
import kotlinx.android.synthetic.main.activity_register.*

class RegisterActivity : BaseActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_register)
        supportActionBar?.setHomeButtonEnabled(true)

        val etRegisterEmail: EditText           = findViewById(R.id.etRegisterEmail)
        val etRegisterPassword: EditText        = findViewById(R.id.etRegisterPassword)
        val etRegisterRepeatPassword: EditText  = findViewById(R.id.etRegisterRepeatPassword)

        btnRegister.setOnClickListener {
            if (isValid(
                    etRegisterEmail,
                    etRegisterPassword,
                    etRegisterRepeatPassword
                )
            ) {
                startActivity(Intent(this, WelcomeActivity::class.java))
            }

        }
    }

    private fun isValid(
        email: EditText,
        password: EditText,
        repeatPassword: EditText
    ): Boolean {
        if (email.text.isEmpty() || password.text.isEmpty() || repeatPassword.text.isEmpty()) {
            Util.showToast(this, getString(R.string.registerFillWarning))
            return false
        } else if (password.text.toString() != repeatPassword.text.toString()) {
            Util.showToast(this, getString(R.string.registerPasswordWarning))
            return false
        }
        Util.showToast(this, "User ${email.text} registered")
        return true
    }
}

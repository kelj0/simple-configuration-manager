package com.mskalnik.simpleconfigurationmanager

import android.content.Intent
import android.os.Bundle
import android.text.Editable
import android.widget.EditText
import com.mskalnik.simpleconfigurationmanager.controller.ApiController
import com.mskalnik.simpleconfigurationmanager.model.User
import com.mskalnik.simpleconfigurationmanager.model.Util
import kotlinx.android.synthetic.main.activity_register.*

class RegisterActivity : BaseActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_register)
        supportActionBar?.setHomeButtonEnabled(true)

        val etRegisterUserName: EditText = findViewById(R.id.etRegisterUserName)
        val etRegisterFirstName: EditText = findViewById(R.id.etRegisterFirstName)
        val etRegisterLastName: EditText = findViewById(R.id.etRegisterLastName)
        val etRegisterEmail: EditText = findViewById(R.id.etRegisterEmail)
        val etRegisterPassword: EditText = findViewById(R.id.etRegisterPassword)
        val etRegisterRepeatPassword: EditText = findViewById(R.id.etRegisterRepeatPassword)

        btnRegister.setOnClickListener {
            registerUser(
                etRegisterUserName.text.toString(),
                etRegisterFirstName.text.toString(),
                etRegisterLastName.text.toString(),
                etRegisterEmail.text.toString(),
                etRegisterPassword.text.toString(),
                etRegisterRepeatPassword.text.toString()
            )
        }
    }

    private fun registerUser(
        userName: String,
        firstName: String,
        lastName: String,
        email: String,
        password: String,
        repeatPassword: String
    ) {
        if (userName.isEmpty()
            || firstName.isEmpty()
            || lastName.isEmpty()
            || email.isEmpty()
            || password.isEmpty()
            || repeatPassword.isEmpty()
        ) {
            Util.showToast(this, getString(R.string.registerFillWarning))
        } else if (!password.equals(repeatPassword)) {
            Util.showToast(this, getString(R.string.registerPasswordWarning))
        } else {
            ApiController.create(User(userName, firstName, lastName, email, password))
            Util.showToast(this, "User $firstName $lastName registered")
            startActivity(Intent(this, WelcomeActivity::class.java))
        }
    }
}

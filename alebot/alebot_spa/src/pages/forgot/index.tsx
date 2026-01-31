import Input from "@/components/input";
import React, { useState } from "react";
import styles from "./styles.module.scss";
import { Button, TextField, Typography } from "@mui/material";
import MessageIcon from "@/assets/svg/MessageIcon";
import useForgot from "./useForgot";
import { formatTime } from "@/helpers/formateTime";
import { forgotStep } from "./forgot.interface";
import { Formik } from "formik";

const ForgotPasswordPage = () => {
  const {
    step,
    seconds,
    resendCode,
    code,
    onCodeChange,
    error,
    email,
    onEmailChange,
    initialValues,
    onSubmit,
    valitdate,
    sendEmailConfirm,
  } = useForgot();

  if (step === forgotStep.email) {
    return (
      <div className={styles.formContent}>
        <div className={styles.links}>
          <Typography>Забыли пароль</Typography>
        </div>
        <Input
          type="email"
          label="Электронная почта"
          variant="filled"
          value={email}
          onChange={onEmailChange}
          error={!!error}
        />
        <span className="errorMessage">{error}</span>
        <Button variant="contained" onClick={sendEmailConfirm}>
          Далее
        </Button>
      </div>
    );
  }

  if (step === forgotStep.confirmation) {
    return (
      <div className={styles.confirmPassword}>
        <Typography className={styles.sectionHeadline}>Сброс пароля</Typography>
        <div className={styles.messageIcon}>
          <MessageIcon />
        </div>
        <Typography className={styles.sectionMessage}>
          Мы отправили письмо о смене пароля на ваш зарегистрированный адрес
          электронной почты: {email.split("@")[0].substring(0, 3)}***@
          {email.split("@")[1]}
        </Typography>
        <div className={styles.sectionActions}>
          <Typography className={styles.timer}>
            Не подтверждено:
            <span>{formatTime(seconds)}</span>
          </Typography>
          <div className={styles.formGroup}>
            <TextField
              type="password"
              label={"Код подтверждения"}
              variant="filled"
              value={code}
              onChange={onCodeChange}
            />
          </div>
          <span className="errorMessage">{error}</span>
          <Button
            variant="contained"
            onClick={resendCode}
            disabled={seconds > 0}
          >
            Выслать письмо повторно
          </Button>
        </div>
      </div>
    );
  }

  if (step === forgotStep.newPassword) {
    return (
      <Formik
        initialValues={initialValues}
        onSubmit={onSubmit}
        validate={valitdate}
      >
        {({
          values,
          errors,
          handleChange,
          handleBlur,
          handleSubmit,
          submitForm,
          touched,
        }) => (
          <form className={styles.formContent} onSubmit={handleSubmit}>
            <div className={styles.links}>
              <Typography>Придумать новый пароль</Typography>
            </div>
            <Input
              disabled
              name="email"
              type="email"
              label="Электронная почта"
              variant="filled"
              value={email}
            />
            <Input
              type="password"
              name="password"
              label="Новый пароль"
              variant="filled"
              onChange={handleChange}
              onBlur={handleBlur}
              error={!!errors.password}
              value={values.password}
            />
            <Input
              name="confirmPassword"
              type="password"
              label="Повторить пароль"
              variant="filled"
              onChange={handleChange}
              onBlur={handleBlur}
              error={!!errors.confirmPassword}
              value={values.confirmPassword}
            />
            {(errors.password && touched.password && (
              <span className="errorMessage">{errors.password}</span>
            )) ||
              (errors.confirmPassword && touched.confirmPassword && (
                <span className="errorMessage">{errors.confirmPassword}</span>
              ))}
            <Button
              variant="contained"
              className={styles.newPass}
              onClick={submitForm}
            >
              Далее
            </Button>
          </form>
        )}
      </Formik>
    );
  }
  return <></>;
};

export default ForgotPasswordPage;

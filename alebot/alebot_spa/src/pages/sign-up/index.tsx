import React, { useState, useEffect } from "react";

import {Box, Button, TextField, Typography} from "@mui/material";
import Input from "@/components/input";

import styles from "./styles.module.scss";
import MessageIcon from "@/assets/svg/MessageIcon";
import { Formik } from "formik";
import useSignUp from "./useSignUp";
import { signUpSteps } from "./signUp.interface";
import { formatTime } from "@/helpers/formateTime";
import MuiPhoneNumber from "mui-phone-number";
import { useRouter } from "next/router";

const SignUpPage = () => {
  const [phone,setPhone] = useState('')

  const {
    initialValues,
    onSubmit,
    valitdate,
    step,
    email,
    seconds,
    resendCode,
    code,
    onCodeChange,
    error
  } = useSignUp();

  const router = useRouter();

  useEffect(() => {
    const token = localStorage.getItem("accessToken");
    if(token){
      router.push("/")
    }
  }, []);
  
  if (step === signUpSteps.registration) {
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
          setFieldValue,
        }) => (
          <form className={styles.formContent} onSubmit={handleSubmit}>
            <Input
              name="fullName"
              type="text"
              label="Имя"
              variant="filled"
              onChange={handleChange}
              onBlur={handleBlur}
              error={!!errors.fullName}
              value={values.fullName}
            />
            <Input
              name="email"
              type="email"
              label="Электронная почта"
              variant="filled"
              onChange={handleChange}
              onBlur={handleBlur}
              error={!!errors.email}
              value={values.email}
            />
            <Input
              name="password"
              type="password"
              label="Пароль"
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
            <Box className={styles.formGroup} sx={{
              borderBottom: '1px solid #eee',
              borderLeft: '1px solid #eee',
              '& .MuiButtonBase-root':{
                background:'none',
                marginTop:'5px',
                '&:hover':{
                  background:'none',
                }
              },
              '& .MuiInputBase-input':{
                background:'none',
                height:'42px !important',
                margin:'0',
                borderLeft:'0 !important'
              },
              '& .MuiInputAdornment-root':{
                paddingLeft:'20px',
              }
            }}>
              <MuiPhoneNumber
                  defaultCountry={'ru'}
                  name='phoneNumber'
                  value={values.phoneNumber}
                  onChange={e => setFieldValue('phoneNumber',e)}
                  autoFormat={false}
                  inputProps={{ maxLength: 13 }}
                  error={!!errors.phoneNumber}
              />
            </Box>
            {(errors.email && touched.email && (
              <span className="errorMessage">{errors.email}</span>
            )) ||
              (errors.password && touched.password && (
                <span className="errorMessage">{errors.password}</span>
              )) ||
              (errors.confirmPassword && touched.confirmPassword && (
                <span className="errorMessage">{errors.confirmPassword}</span>
              ))  ||
              (errors.phoneNumber && touched.phoneNumber && (
                <span className="errorMessage">{errors.phoneNumber}</span>
              ))  ||
              (errors.fullName && touched.fullName && (
                <span className="errorMessage">{errors.fullName}</span>
              ))}
            <Typography className={styles.terms}>
              Нажимая <span>«Далее»,</span> вы принимаете
              <a href="https://docs.google.com/document/d/1esAdDauzeZqH1I1Au6PUMMT4agJ8Yiq9/edit?usp=drive_link&ouid=100253606333501006370&rtpof=true&sd=true" target="_blank">пользовательское соглашение </a> и соглашаетесь на
              обработку вашей персональной информации
              <a href="https://docs.google.com/document/d/1ZdhhW3VjbM4x1qazem0FGTGqeg_TKhEa/edit?usp=drive_link&ouid=100253606333501006370&rtpof=true&sd=true" target="_blank"> на условиях политики конфиденциальности.</a>
            </Typography>
            <Button variant="contained" onClick={submitForm}>
              Далее
            </Button>
          </form>
        )}
      </Formik>
    );
  }

  if (step === signUpSteps.confirmation) {
    return (
      <div className={styles.confirmPassword}>
        <Typography className={styles.sectionHeadline}>
          Подтвердите email
        </Typography>
        <div className={styles.messageIcon}>
          <MessageIcon />
        </div>
        <Typography className={styles.sectionMessage}>
          Мы отправили письмо на ваш зарегистрированный адрес электронной почты:
          {email.split("@")[0].substring(0, 3)}***@{email.split("@")[1]}
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
            disabled={seconds !== 0}
          >
            Выслать письмо повторно
          </Button>
        </div>
      </div>
    );
  }
  return <></>;
};

export default SignUpPage;

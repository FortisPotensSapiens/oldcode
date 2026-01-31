import {
  PASSWORD_MISMATCH,
  REQUIRED_TEXT,
  WRONG_CODE,
  WRONG_EMAIL_FORMAT, WRONG_PHONE,
} from "@/constants/messages";
import { emailValidator } from "@/helpers/emailValidator";
import {
  IOnChange,
  ISignUpErrors,
  ISignUpValues,
  signUpSteps,
} from "./signUp.interface";
import {
  confirmEmailRequest,
  sendConfirmCodeRequest,
  signInRequest,
  signUpRequest,
} from "@/service/auth";
import { useEffect, useState } from "react";
import { useRouter } from "next/router";
import { IConfirmEmail } from "../forgot/forgot.interface";
import { ISignInValues } from "../sign-in/singIn.interface";
import * as amplitude from '@amplitude/analytics-browser';
import { Message } from "@mui/icons-material";

const useSignUp = () => {
  const [seconds, setSeconds] = useState<number>(0);
  const [step, setStep] = useState(signUpSteps.registration);
  const [code, setCode] = useState<string>("");
  const [email, setEmail] = useState<string>("");
  const [error, setError] = useState("");
  const [password, setPassword] = useState("");
  const router = useRouter()
  const initialValues: ISignUpValues = {
    email: "",
    password: "",
    confirmPassword: "",
    phoneNumber: '',
  };

  const resendCode = () => {
    sendConfirmCodeRequest({ email });
    setSeconds(20);
  };

  const onSubmit = async ({ email, password, phoneNumber, fullName }: ISignUpValues) => {
    let data = { email, password, phoneNumber, fullName }
    if (router.query.referrer) {
      data['refererId'] = router.query.referrer
    }
    const registrationQueryParams = localStorage.getItem("registrationQueryParams");
    data['registrationQueryParams'] = registrationQueryParams;
    const responseStatus = await signUpRequest(data);
    if (responseStatus === 200) {
      try {
        console.log(jivo_api);
        jivo_api.setContactInfo({
          name: data.fullName,
          email: data.email,
          phone: data.phoneNumber,
          description: registrationQueryParams
        });
        jivo_api.sendMessage("Клиент начал процесс регистрации!");
      } catch (error) {
        console.log(error);
      }
      setStep(signUpSteps.confirmation);
      setEmail(email);
      setPassword(password);
    }
  };
  const valitdate = (values: ISignUpValues) => {
    const errors: ISignUpErrors = {};
    if (!values.email) {
      errors.email = REQUIRED_TEXT;
    } else if (emailValidator(values.email)) {
      errors.email = WRONG_EMAIL_FORMAT;
    }
    if (!values.password) {
      errors.password = REQUIRED_TEXT;
    }
    if (values.confirmPassword !== values.password) {
      errors.confirmPassword = PASSWORD_MISMATCH;
    }
    if (!(values.phoneNumber && values.phoneNumber.length > 5))
      errors.phoneNumber = 'Укажите номер телефона!';
    if (!values.fullName)
      errors.fullName = REQUIRED_TEXT;
    return errors;
  };

  const onCodeChange: IOnChange = ({ target: { value } }) => setCode(value);

  useEffect(() => {
    if (step === signUpSteps.confirmation) {
      resendCode();
    }
  }, [step]);

  useEffect(() => {
    let interval: NodeJS.Timeout;

    if (seconds !== 0 && step === signUpSteps.confirmation) {
      interval = setInterval(() => {
        setSeconds((prevSeconds: number) => prevSeconds - 1);
      }, 1000);
    }
    if (seconds === 0) {
      //@ts-ignore
      clearInterval(interval);
    }

    return () => clearInterval(interval);
  }, [step, seconds]);

  const confirmEmail = async (data: IConfirmEmail) => {
    const response = await confirmEmailRequest(data);
    if (response.status === 200) {
      amplitude.setUserId(email);
      amplitude.track('Sign Up');
      await signIn({
        email: email,
        password: password
      });
    } else {
      setError(WRONG_CODE);
    }
    setCode("");
  };

  const signIn = async (values: ISignInValues) => {
    const response = await signInRequest(values);

    if (response?.accessToken && response?.refreshToken) {
      localStorage.setItem("accessToken", response.accessToken);
      localStorage.setItem("refreshToken", response.refreshToken);
      router.push("/products");
    }
  };

  useEffect(() => {
    if (code.length === 6) {
      confirmEmail({ email, code });
    }
  }, [code, email]);
  return {
    initialValues,
    onSubmit,
    valitdate,
    step,
    setStep,
    email,
    seconds,
    resendCode,
    code,
    onCodeChange,
    error,
  };
};

export default useSignUp;

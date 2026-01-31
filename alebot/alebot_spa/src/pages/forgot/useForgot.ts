import { useEffect, useState } from "react";
import {
  IConfirmEmail,
  IForgotErrors,
  IForgotValues,
  IOnChange,
  forgotStep,
} from "./forgot.interface";
import { useRouter } from "next/router";
import {
  checkPasswordResetCodeRequest,
  resetPasswordRequest,
  sendResetPasswordCodeRequest,
} from "@/service/auth";
import {
  PASSWORD_MISMATCH,
  REQUIRED_TEXT,
  WRONG_CODE,
  WRONG_EMAIL_FORMAT,
} from "@/constants/messages";
import { ISignUpErrors, ISignUpValues } from "../sign-up/signUp.interface";
import { emailValidator } from "@/helpers/emailValidator";

const useForgot = () => {
  const [seconds, setSeconds] = useState<number>(0);
  const [step, setStep] = useState(forgotStep.email);
  const [code, setCode] = useState<string>("");
  const [email, setEmail] = useState<string>("");
  const [error, setError] = useState("");
  const router = useRouter();

  const initialValues: IForgotValues = {
    email,
    password: "",
    confirmPassword: "",
  };

  const onCodeChange: IOnChange = ({ target: { value } }) => setCode(value);

  const onEmailChange: IOnChange = ({ target: { value } }) => {
    setError("");
    setEmail(value);
  };

  const resendCode = () => {
    sendResetPasswordCodeRequest({ email });
    setSeconds(20);
  };

  const sendEmailConfirm = () => {
    if (!emailValidator(email)) {
      setStep(forgotStep.confirmation);
    } else {
      setError(WRONG_EMAIL_FORMAT);
    }
  };

  const confirmEmail = async (data: IConfirmEmail) => {
    const response = await checkPasswordResetCodeRequest(data);
    if (response) {
      setStep(forgotStep.newPassword);
    } else {
      setError(WRONG_CODE);
      setCode("");
    }
  };

  const valitdate = (values: IForgotValues) => {
    const errors: IForgotErrors = {};
    if (!values.password) {
      errors.password = REQUIRED_TEXT;
    }
    if (values.confirmPassword !== values.password) {
      errors.confirmPassword = PASSWORD_MISMATCH;
    }
    return errors;
  };

  const onSubmit = async ({ email, password }: IForgotValues) => {
    const response = await resetPasswordRequest({
      code,
      email,
      password,
    });
    if (response.status === 200) {
      router.push("/sign-in");
    }
  };

  useEffect(() => {
    if (step === forgotStep.confirmation) {
      resendCode();
    }
  }, [step]);

  useEffect(() => {
    if (code.length === 6) {
      confirmEmail({ email, code });
    }
  }, [code, email]);

  useEffect(() => {
    let interval: NodeJS.Timeout;

    if (seconds !== 0 && step === forgotStep.confirmation) {
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

  return {
    step,
    setStep,
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
  };
};

export default useForgot;

import styled from '@emotion/styled';
import { Alert, Spin } from 'antd';
import { FC, useCallback, useEffect, useState } from 'react';
import { v4 as uuidv4 } from 'uuid';

import { usePostCheckVerificationCode } from '~/api/v1/usePostCheckVerificationCode';
import { usePostSendVerificationCode } from '~/api/v1/usePostSendVerificationCode';

import SMSConfirm, { SMSConfirmProps } from './SMSConfirm';

const StyledSpinerContainer = styled.div`
  display: flex;
  justify-content: center;
  margin-right: 2rem;
`;

const SMSConfirmContainer: FC<
  Omit<SMSConfirmProps, 'onSubmit' | 'onResend'> & {
    onSuccess: (code: string) => void;
  }
> = ({ milisecond, symbols, phoneNumber, onSuccess }) => {
  const sendVerificationMutation = usePostSendVerificationCode();
  const checkVerificationCode = usePostCheckVerificationCode();
  const [formId, setFormId] = useState(uuidv4());
  const [code, setCode] = useState('');

  useEffect(() => {
    if (phoneNumber) {
      sendVerificationMutation.mutate({
        phone: phoneNumber,
      });
    }
  }, []);

  const verifyCode = useCallback(
    (code: string) => {
      checkVerificationCode.mutate({
        phone: phoneNumber,
        code,
      });
    },
    [phoneNumber, checkVerificationCode],
  );

  const onResendCallback = () => {
    sendVerificationMutation.mutate({
      phone: phoneNumber,
    });
  };

  const onSubmitCallback = (code: string) => {
    setCode(code);
    verifyCode(code);
  };

  const onChangeCallback = () => {
    checkVerificationCode.reset();
  };

  useEffect(() => {
    if ((checkVerificationCode.data as unknown as boolean) === false) {
      setFormId(uuidv4());

      return;
    }

    if ((checkVerificationCode.data as unknown as boolean) === true) {
      onSuccess(code);
    }
  }, [checkVerificationCode.data]);

  return (
    <>
      {sendVerificationMutation.isSuccess ? (
        <SMSConfirm
          key={formId}
          milisecond={milisecond}
          symbols={symbols}
          phoneNumber={phoneNumber}
          onSubmit={onSubmitCallback}
          onChange={onChangeCallback}
          onResend={onResendCallback}
        />
      ) : undefined}

      {sendVerificationMutation.isLoading || checkVerificationCode.isLoading ? (
        <StyledSpinerContainer>
          <Spin />
        </StyledSpinerContainer>
      ) : undefined}

      {(checkVerificationCode.data as unknown as boolean) === false ? (
        <Alert message="Не верный код" type="error" />
      ) : undefined}

      {sendVerificationMutation.isError ? (
        <Alert message="Ошибка отправки проверочного кода" type="error" />
      ) : undefined}
    </>
  );
};

export default SMSConfirmContainer;

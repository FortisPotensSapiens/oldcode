import styled from '@emotion/styled';
import { Button, Col, Input, InputRef, Row, Typography } from 'antd';
import React, { createRef, FC, useEffect, useState } from 'react';
import Countdown from 'react-countdown';
import { v4 as uuidv4 } from 'uuid';

const { Text } = Typography;

const StyledInput = styled(Input)`
  font-size: 2rem;
  max-width: 50px;

  @media (max-width: 900px) {
    font-size: 1.5rem;
    max-width: 40px;
  }
`;

const Inputs = styled.div`
  padding-top: 1rem;
  max-width: 500px;
  margin: 0px auto;
  text-align: center;
`;

const StyledTitle = styled.div`
  text-align: center;
  padding-right: 1rem;
`;

const StyledCounter = styled.div`
  opacity: 0.5;
  text-align: center;
  padding: 1rem 0;
`;

const StyledRepeactCounterButton = styled.div`
  display: flex;
  padding-top: 1rem;
  padding-bottom: 1rem;
  justify-content: center;
`;

export interface SMSConfirmProps {
  milisecond?: number;
  symbols?: number;
  phoneNumber: string;
  onSubmit: (code: string) => void;
  onChange?: () => void;
  onResend: () => void;
  key?: string;
}

const SMSConfirm: FC<SMSConfirmProps> = ({
  milisecond = 15000,
  symbols = 6,
  phoneNumber,
  onSubmit,
  key,
  onChange,
  onResend,
}) => {
  const [inputRefsArray] = useState(() => Array.from({ length: symbols }, () => createRef<InputRef>()));
  const [counterKey, setCounterKey] = useState(uuidv4());

  useEffect(() => {
    inputRefsArray[0].current?.focus();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const restart = () => {
    setCounterKey(uuidv4());
    onResend();
  };

  useEffect(() => {
    if (key) {
      inputRefsArray.forEach((item) => {
        if (item.current) {
          if (item.current.input) {
            item.current.input.value = '';
          }
        }
      });
    }
  }, [key]);

  const renderer = ({
    minutes,
    seconds,
    completed,
  }: {
    hours: number;
    minutes: number;
    seconds: number;
    completed: boolean;
  }) => {
    if (completed) {
      return (
        <StyledRepeactCounterButton>
          <Button onClick={restart}>Выслать повторно</Button>
        </StyledRepeactCounterButton>
      );
    }

    return (
      <StyledCounter>
        {minutes.toLocaleString('ru-RU', { minimumIntegerDigits: 2, useGrouping: false })}:
        {seconds.toLocaleString('ru-RU', { minimumIntegerDigits: 2, useGrouping: false })}
      </StyledCounter>
    );
  };

  return (
    <>
      <Typography>
        <StyledTitle>
          <Text>Для подтверждения Вашего номера телефона {phoneNumber} мы выслали Вам проверочный код</Text>
        </StyledTitle>

        <Inputs>
          <Row justify="space-around">
            {inputRefsArray.map((ref, i) => {
              return (
                // eslint-disable-next-line react/no-array-index-key
                <Col key={i}>
                  <StyledInput
                    type="text"
                    pattern="\d*"
                    maxLength={1}
                    ref={ref}
                    onChange={(e) => {
                      const nextRef = inputRefsArray[i + 1];

                      if (symbols - 1 === i) {
                        let code = '';

                        inputRefsArray.forEach((item) => {
                          code += String(item.current?.input?.value);
                        });

                        onSubmit(code);

                        return;
                      }

                      if (nextRef) {
                        nextRef.current?.input?.focus();
                      }

                      onChange?.();
                    }}
                  />
                </Col>
              );
            })}
          </Row>
        </Inputs>
      </Typography>
      <Countdown date={Date.now() + milisecond} renderer={renderer} key={counterKey} />
    </>
  );
};

export default SMSConfirm;

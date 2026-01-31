import styled from '@emotion/styled';
import { FC } from 'react';
import { Controller, ControllerProps, useFormContext } from 'react-hook-form';
import PhoneInput from 'react-phone-number-input';
import flags from 'react-phone-number-input/flags';
import ru from 'react-phone-number-input/locale/ru.json';

import FloatLabel from '../FloatLabel/FloatLabel';

import 'react-phone-number-input/style.css';

type FormFieldPhoneProps = Pick<ControllerProps, 'rules' | 'name'> & {
  label: string;
  disabled?: boolean;
};

const StyledFloatLabel = styled(FloatLabel)`
  & .PhoneInputInput {
    padding: 18px 12px 12px 15px;
    border: 1px solid #d9d9d9;
    border-radius: 6px;
    transition: all 0.2s;
  }

  & .PhoneInputInput:hover,
  & .PhoneInputInput:focus {
    border: 1px solid #8a1656 !important;
    box-shadow: 0 0 0 2px rgb(61 5 29 / 34%);
    outline: 0;
  }
`;

const FormFieldPhone: FC<FormFieldPhoneProps> = ({ disabled = false, name, rules, ...props }) => {
  const { control } = useFormContext();

  return (
    <Controller
      control={control}
      name={name}
      render={({ field }) => {
        return (
          <StyledFloatLabel label={props.label} required value={field.value} left={55}>
            <PhoneInput smartCaret limitMaxLength flags={flags} labels={ru} defaultCountry="RU" {...field} />
          </StyledFloatLabel>
        );
      }}
      rules={rules}
    />
  );
};

export { FormFieldPhone };
export type { FormFieldPhoneProps };

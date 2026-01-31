import { FC } from 'react';
import { Controller, ControllerProps, useFormContext } from 'react-hook-form';

type FormControllerProps = Omit<ControllerProps, 'control'> & { required?: string };

const FormController: FC<FormControllerProps> = (props) => {
  const { control } = useFormContext();

  return <Controller {...props} control={control} />;
};

export { FormController };
export type { FormControllerProps };

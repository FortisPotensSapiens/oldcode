import { Grid, TextField } from '@mui/material';
import { FC } from 'react';
import { Controller, useFormContext } from 'react-hook-form';

import { SupportFormData } from './types';

type SupportFormProps = { disabled?: boolean };

const SupportForm: FC<SupportFormProps> = ({ children, disabled }) => {
  const {
    control,
    formState: { errors },
  } = useFormContext<SupportFormData>();

  return (
    <Grid container spacing={4}>
      <Grid item xs={12}>
        <Controller
          control={control}
          name="email"
          render={({ field }) => (
            <TextField
              disabled={disabled}
              error={!!errors.email}
              fullWidth
              helperText={errors.email?.message}
              label="Электронная почта"
              {...field}
            />
          )}
          rules={{ pattern: /^[^\s@]+@[^\s@]+\.[^\s@]+$/, required: true }}
        />
      </Grid>

      <Grid item xs={12}>
        <Controller
          control={control}
          name="name"
          render={({ field }) => <TextField disabled={disabled} fullWidth label="Имя (необязательно)" {...field} />}
        />
      </Grid>

      <Grid item xs={12}>
        <Controller
          control={control}
          name="message"
          render={({ field }) => (
            <TextField
              disabled={disabled}
              error={!!errors.message}
              fullWidth
              helperText={errors?.message?.message}
              label="Сообщение"
              multiline
              rows={4}
              {...field}
            />
          )}
          rules={{ required: true }}
        />
      </Grid>

      <Grid item xs={12}>
        {children}
      </Grid>
    </Grid>
  );
};

export { SupportForm };

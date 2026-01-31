import { LoadingButton } from '@mui/lab';
import { Box, Grid, Paper } from '@mui/material';
import { useSnackbar } from 'notistack';
import { FC, useEffect } from 'react';
import { FormProvider, useForm } from 'react-hook-form';

import { usePutMyPassword } from '~/api';
import { FormField } from '~/components';
import { useDownSm } from '~/hooks';
import { PasswordUpdateModel } from '~/types';

type FormProps = { onUpdated: () => void };

const UpdatePasswordForm: FC<FormProps> = ({ onUpdated }) => {
  const submitForm = usePutMyPassword();
  const { enqueueSnackbar } = useSnackbar();
  const isXs = useDownSm();

  const methods = useForm<PasswordUpdateModel>({
    mode: 'all',
  });

  const submitHandler = (event: PasswordUpdateModel) => {
    submitForm.mutate(event);
  };

  useEffect(() => {
    if (submitForm.isError) {
      enqueueSnackbar('Ошибка смены пароля', {
        variant: 'error',
      });
      onUpdated();
    }
  }, [submitForm.isError, enqueueSnackbar, onUpdated]);

  useEffect(() => {
    if (submitForm.isSuccess) {
      enqueueSnackbar('Пароль успешно изменен', {
        variant: 'success',
      });
      onUpdated();
    }
  }, [submitForm.isSuccess, enqueueSnackbar, onUpdated]);

  return (
    <Box>
      <FormProvider {...methods}>
        <Box component="form" onSubmit={methods.handleSubmit(submitHandler)}>
          <Grid container spacing={3}>
            <Grid item sm xs={12}>
              <Box borderRadius={{ sm: 2.5, xs: 0 }} component={Paper} overflow="hidden">
                <Grid alignItems="flex-start" container direction="row">
                  <Box component={Grid} container item md p={3} sm={12} spacing={3} xs={12}>
                    <Grid item xs={12}>
                      <FormField
                        label="Пароль"
                        name="newPassword"
                        rules={{
                          required: { message: 'Укажите пароль', value: true },
                        }}
                        type="password"
                      />
                    </Grid>

                    <Grid item xs={12}>
                      <FormField
                        label="Подтверждение пароля"
                        name="confirmPassword"
                        rules={{
                          required: { message: 'Укажите подтверждение пароля', value: true },
                        }}
                        type="password"
                      />
                    </Grid>

                    <Grid item xs={12}>
                      <FormField
                        label="Старый пароль"
                        name="oldPassword"
                        rules={{
                          required: { message: 'Укажите старый пароль', value: true },
                        }}
                        type="password"
                      />
                    </Grid>

                    <Grid item xs={12}>
                      <LoadingButton
                        fullWidth={isXs}
                        loading={submitForm.isLoading}
                        size="large"
                        type="submit"
                        variant="contained"
                      >
                        Сохранить изменения
                      </LoadingButton>
                    </Grid>
                  </Box>
                </Grid>
              </Box>
            </Grid>
          </Grid>
        </Box>
      </FormProvider>
    </Box>
  );
};

export default UpdatePasswordForm;

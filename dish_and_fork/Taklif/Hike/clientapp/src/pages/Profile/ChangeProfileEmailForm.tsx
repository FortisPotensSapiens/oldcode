import { LoadingButton } from '@mui/lab';
import { Box, Grid, Paper } from '@mui/material';
import { useSnackbar } from 'notistack';
import { FC, useEffect } from 'react';
import { FormProvider, useForm } from 'react-hook-form';

import { usePutMyEmail } from '~/api';
import { FormField } from '~/components';
import { useDownSm } from '~/hooks';
import { EmailUpdateModel } from '~/types';

type FormProps = { data?: EmailUpdateModel; onUpdated: () => void };

const ChangeProfileEmailForm: FC<FormProps> = ({ data, onUpdated }) => {
  const submitForm = usePutMyEmail();
  const { enqueueSnackbar } = useSnackbar();
  const isXs = useDownSm();

  const methods = useForm<EmailUpdateModel>({
    defaultValues: data,
    mode: 'all',
  });

  const submitHandler = (event: EmailUpdateModel) => {
    submitForm.mutate(event);
  };

  useEffect(() => {
    if (submitForm.isError) {
      enqueueSnackbar('Ошибка смены email', {
        variant: 'error',
      });
      onUpdated();
    }
  }, [submitForm.isError, enqueueSnackbar, onUpdated]);

  useEffect(() => {
    if (submitForm.isSuccess) {
      enqueueSnackbar('На вашу почту выслано письмо для подтверждения смены почты', {
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
                        label="Email"
                        name="newEmail"
                        rules={{
                          pattern: { message: 'В email запрешены пробелы', value: /^[^\s]+$/ },
                          required: { message: 'Укажите логин пользователя', value: true },
                        }}
                        type="email"
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

export default ChangeProfileEmailForm;

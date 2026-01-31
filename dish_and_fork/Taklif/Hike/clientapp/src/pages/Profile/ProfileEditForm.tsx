import { LoadingButton } from '@mui/lab';
import { Box, Grid, Paper } from '@mui/material';
import { useSnackbar } from 'notistack';
import { FC, useEffect } from 'react';
import { FormProvider, useForm } from 'react-hook-form';

import { usePutMyUserProfile } from '~/api';
import { FormField } from '~/components';
import { useDownSm } from '~/hooks';
import { UserProfileUpdateModel } from '~/types';

type ProfileEditFormProps = { data?: Pick<UserProfileUpdateModel, 'userName'>; onUpdated: () => void };

const ProfileEditForm: FC<ProfileEditFormProps> = ({ data, onUpdated }) => {
  const putMyUserProfile = usePutMyUserProfile();
  const { enqueueSnackbar } = useSnackbar();
  const isXs = useDownSm();

  const methods = useForm<UserProfileUpdateModel>({
    defaultValues: data,
    mode: 'all',
  });

  const submitHandler = (event: UserProfileUpdateModel) => {
    putMyUserProfile.mutate(event);
  };

  useEffect(() => {
    if (putMyUserProfile.isError) {
      enqueueSnackbar('Ошибка обновления профиля', {
        variant: 'error',
      });
      onUpdated();
    }
  }, [putMyUserProfile.isError, enqueueSnackbar, onUpdated]);

  useEffect(() => {
    if (putMyUserProfile.isSuccess) {
      enqueueSnackbar('Профиль обновлен', {
        variant: 'success',
      });
      onUpdated();
    }
  }, [putMyUserProfile.isSuccess, enqueueSnackbar, onUpdated]);

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
                        label="Логин"
                        name="userName"
                        rules={{
                          pattern: { message: 'В имени пользователя запрешены пробелы', value: /^[^\s]+$/ },
                          required: { message: 'Укажите логин пользователя', value: true },
                        }}
                      />
                    </Grid>

                    <Grid item xs={12}>
                      <LoadingButton
                        fullWidth={isXs}
                        loading={putMyUserProfile.isLoading}
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

export default ProfileEditForm;

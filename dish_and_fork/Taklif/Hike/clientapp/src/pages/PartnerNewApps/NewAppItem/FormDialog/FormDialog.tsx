import { LoadingButton } from '@mui/lab';
import { Box, Button, Dialog, DialogContent, DialogProps, DialogTitle, Grid } from '@mui/material';
import { Typography } from 'antd';
import { FC } from 'react';
import { Controller, FormProvider, useForm } from 'react-hook-form';

import { FormField, FormFieldDatePicker } from '~/components';
import { FormTextField } from '~/views/PartnerProductView/FormTextField';
import { Images } from '~/views/PartnerProductView/Images';

import { SubmitCallback } from './types';
import { FormData, useSubmit } from './useSubmit';

type FormDialogProps = Pick<DialogProps, 'open'> & {
  applicationId: string;
  onComplete?: SubmitCallback;
  onClose?: () => void;
};

const FormDialog: FC<FormDialogProps> = ({ applicationId, onClose, onComplete, open }) => {
  const methods = useForm<FormData>({
    defaultValues: {
      images: [],
    },
  });
  const [submitHanlder, { isLoading }] = useSubmit(applicationId, onComplete);

  const handleDelete = (index: number) => {
    const { images } = methods.getValues();

    if (images) {
      images.splice(index, 1);

      methods.setValue('images', images);
    }
  };

  return (
    <FormProvider {...methods}>
      <Dialog fullWidth maxWidth="sm" onClose={onClose} open={open}>
        <form onSubmit={methods.handleSubmit(submitHanlder)}>
          <DialogTitle>Откликнуться на заявку</DialogTitle>

          <DialogContent>
            <Box component={Grid} container mt={0} spacing={2}>
              <Grid item xs={12}>
                <FormField
                  label="Цена"
                  name="sum"
                  rules={{
                    required: {
                      message: 'Укажите цену',
                      value: true,
                    },
                  }}
                  type="number"
                />
              </Grid>

              <Grid item xs={12}>
                <FormFieldDatePicker
                  label="Дата"
                  name="date"
                  rules={{
                    required: {
                      message: 'Укажите дату готовности',
                      value: true,
                    },
                  }}
                />
              </Grid>

              <Grid item md={12} xs={12}>
                <FormTextField
                  valueAsFloat
                  label="Вес порции брутто"
                  name="servingGrossWeightInKilograms"
                  rules={{ required: true }}
                  required
                  InputProps={{ endAdornment: 'г' }}
                />
              </Grid>

              <Grid item xs={12}>
                <FormField label="Комментарии" multiline name="description" />
              </Grid>

              <Grid item xs={12}>
                <Typography.Title
                  level={4}
                  style={{ fontSize: '100%', paddingTop: '0', fontWeight: 'normal', marginTop: '0', opacity: '0.7' }}
                >
                  Загрузить примеры работ
                </Typography.Title>
                <Controller
                  name="images"
                  render={({ field: { ref, ...field } }) => {
                    return (
                      <Images
                        isDraggingEnabled={false}
                        onDelete={handleDelete}
                        max={999}
                        showCrow={false}
                        cropping={false}
                        {...field}
                        containerProps={{
                          display: 'flex',
                          overflow: 'scroll',
                        }}
                        itemProps={{
                          paddingRight: 2,
                        }}
                      />
                    );
                  }}
                />
              </Grid>
            </Box>
          </DialogContent>

          <DialogContent>
            <Grid container spacing={2}>
              <Grid item order={0} sm={6} xs={12}>
                <Button
                  color="primary"
                  disabled={isLoading}
                  fullWidth
                  onClick={onClose}
                  size="large"
                  variant="outlined"
                >
                  Отмена
                </Button>
              </Grid>

              <Grid item order={{ sm: 0, xs: -1 }} sm={6} xs={12}>
                <LoadingButton
                  color="primary"
                  fullWidth
                  loading={isLoading}
                  size="large"
                  type="submit"
                  variant="contained"
                >
                  Откликнуться
                </LoadingButton>
              </Grid>
            </Grid>
          </DialogContent>
        </form>
      </Dialog>
    </FormProvider>
  );
};

export { FormDialog };
export type { FormDialogProps };

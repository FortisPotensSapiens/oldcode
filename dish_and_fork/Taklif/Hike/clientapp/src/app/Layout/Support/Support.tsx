import { Close } from '@mui/icons-material';
import { LoadingButton } from '@mui/lab';
import { Alert, Box, Button, Dialog, DialogContent, IconButton } from '@mui/material';
import { FC, useState } from 'react';
import { FormProvider, useForm } from 'react-hook-form';

import { useSupportActions, useSupportState } from '~/contexts';
import { useDownSm } from '~/hooks';

import { FileField } from './FileField';
import { SupportForm } from './SupportForm';
import { SupportFormData } from './types';
import { useReset } from './useReset';
import { useSubmit } from './useSubmit';

const Support: FC = () => {
  const methods = useForm<SupportFormData>({
    defaultValues: {
      email: '',
      message: '',
      name: '',
    },
  });

  const { clearErrors, handleSubmit, reset } = methods;
  const visible = useSupportState();
  const isXs = useDownSm();
  const { hide } = useSupportActions();
  const [files, setFiles] = useState<File[]>(() => []);
  const [submit, { isError, isLoading, isSuccess }] = useSubmit(!visible, reset, setFiles, files);

  useReset(visible, clearErrors);

  return (
    <Dialog fullScreen={isXs} maxWidth="xs" onClose={hide} open={visible}>
      <Box component={DialogContent} display="flex" flexDirection="column" height={1}>
        <Box component={IconButton} mr={2} mt={2} onClick={hide} position="absolute" right={0} top={0}>
          <Close />
        </Box>

        {isSuccess ? (
          <Box display="flex" flexDirection="column" height={1}>
            <Box textAlign="center" typography="h5">
              Сообщение отправлено
            </Box>

            <Box color="text.secondary" flexGrow={1} mb={4} mt={3} textAlign="center">
              Благодарим за обращение. Наш специалист свяжется с Вами в ближайшее время.
            </Box>

            <Button color="success" fullWidth onClick={hide} size="large" variant="contained">
              Закрыть
            </Button>
          </Box>
        ) : (
          <FormProvider {...methods}>
            <Box component="form" display="flex" flexDirection="column" height={1} onSubmit={handleSubmit(submit)}>
              <Box typography="h5">Связаться с нами</Box>

              <Box color="text.secondary" maxWidth={270} mt={1}>
                Наш специалист свяжется с Вами в ближайшее время.
              </Box>

              <Box flexGrow={1} mb={3} mt={2}>
                <SupportForm disabled={isLoading}>
                  <FileField disabled={isLoading} files={files} name="name" onChange={setFiles} />
                </SupportForm>
              </Box>

              {isError && (
                <Box component={Alert} mb={2} severity="error">
                  Во время отправки сообщения произошла ошибка.
                </Box>
              )}

              <LoadingButton fullWidth loading={isLoading} size="large" type="submit" variant="contained">
                Отправить
              </LoadingButton>
            </Box>
          </FormProvider>
        )}
      </Box>
    </Dialog>
  );
};

export { Support };

import { useSnackbar } from 'notistack';
import { useEffect } from 'react';
import { Controller, FormProvider, useForm } from 'react-hook-form';

import { useGetMyUserProfile } from '~/api';
import { usePostNewIndividualOrderComment } from '~/api/v1/usePostNewIndividualOrderComment';
import Avatar from '~/components/Avatar/Avatar';

import { StyledContainer, StyledInput, StyledSendIcon } from './styled';

interface Form {
  text: string;
  offerId: string;
}

const AddCommentForm = ({ offerId }: { offerId: string }) => {
  const { enqueueSnackbar } = useSnackbar();
  const methods = useForm<Form>({
    defaultValues: {
      text: '',
      offerId,
    },
  });
  const submitForm = usePostNewIndividualOrderComment();
  const { data: userInfo } = useGetMyUserProfile();

  const onSubmit = (data: Form) => {
    submitForm.mutate(data);
  };

  useEffect(() => {
    if (submitForm.isSuccess) {
      methods.setValue('text', '');

      enqueueSnackbar('Ваш коментарий отправлен.', {
        variant: 'success',
      });
    }
  }, [enqueueSnackbar, methods, submitForm.isSuccess]);

  return (
    <FormProvider {...methods}>
      <StyledContainer>
        <Controller
          name="text"
          control={methods.control}
          render={({ field }) => (
            <StyledInput
              endAdornment={<StyledSendIcon onClick={methods.handleSubmit(onSubmit)} />}
              fullWidth
              multiline
              placeholder="Отправить сообщение.."
              {...field}
            />
          )}
        />

        <Avatar userName={userInfo?.userName ?? ''} />
      </StyledContainer>
    </FormProvider>
  );
};

export { AddCommentForm };

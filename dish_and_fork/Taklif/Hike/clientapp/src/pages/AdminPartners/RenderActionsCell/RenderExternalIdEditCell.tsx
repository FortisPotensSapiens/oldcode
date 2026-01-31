import styled from '@emotion/styled';
import { yupResolver } from '@hookform/resolvers/yup';
import { CheckOutlined } from '@mui/icons-material';
import { Button, Input } from 'antd';
import { FC, useCallback, useEffect } from 'react';
import { useDetectClickOutside } from 'react-detect-click-outside';
import { Controller, useForm } from 'react-hook-form';
import * as yup from 'yup';

import { usePutAdminExternalId } from '~/api/v1/usePutAdminExternalId';

const schema = yup.object({
  externalId: yup.string().required(),
});

const StyledSubmitButton = styled(Button)({
  marginLeft: '0.3rem',
});

const Container = styled('div')({
  display: 'inline-flex',
});

type Form = {
  externalId: string;
};

const RenderExternalIdEditCell: FC<{
  externalId?: string | null;
  id: string;
  onClose: () => void;
  onSuccess: () => void;
}> = ({ externalId, id, onClose, onSuccess }) => {
  const handleClickOutside = useCallback(() => {
    onClose();
  }, [onClose]);
  const mutation = usePutAdminExternalId();
  const ref = useDetectClickOutside({ onTriggered: handleClickOutside });
  const form = useForm<Form>({
    mode: 'all',
    defaultValues: {
      externalId: externalId ?? '',
    },
    resolver: yupResolver(schema),
  });

  const handleSubmitButton = useCallback(
    (data: Form) => {
      mutation.mutate({
        id,
        externalId: data.externalId,
      });
    },
    [id, mutation],
  );

  useEffect(() => {
    if (mutation.isSuccess) {
      onClose();
      onSuccess();
    }
  }, [mutation.isSuccess, onClose, onSuccess]);

  return (
    <Container ref={ref}>
      <Controller
        name="externalId"
        control={form.control}
        render={({ field }) => {
          return <Input {...field} />;
        }}
      />
      <StyledSubmitButton
        disabled={!form.formState.isValid || !form.formState.isDirty}
        icon={<CheckOutlined />}
        onClick={form.handleSubmit(handleSubmitButton)}
      />
    </Container>
  );
};

export { RenderExternalIdEditCell };

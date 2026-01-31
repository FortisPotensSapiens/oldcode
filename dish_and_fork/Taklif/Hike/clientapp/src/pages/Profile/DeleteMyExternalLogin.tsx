import ClearIcon from '@mui/icons-material/Clear';
import { Box } from '@mui/material';
import { useSnackbar } from 'notistack';
import { FC, useEffect } from 'react';

import { useDeleteMyExternalLogin } from '~/api';
import { LoginRemoveModel, UserLoginInfo } from '~/types';

import { StyledIconButton } from './StyledIconButton';

type DeleteMyExternalLoginProps = Pick<UserLoginInfo, 'providerDisplayName'> &
  LoginRemoveModel & {
    disabled?: boolean;
    onUpdated: () => void;
  };

const DeleteMyExternalLogin: FC<DeleteMyExternalLoginProps> = ({
  children,
  disabled,
  onUpdated,
  providerDisplayName,
  ...data
}) => {
  const { isError, isLoading, isSuccess, mutate } = useDeleteMyExternalLogin();
  const { enqueueSnackbar } = useSnackbar();

  const clickHandler = () => {
    mutate(data);
  };

  useEffect(() => {
    if (isError) {
      enqueueSnackbar('Ошибка', {
        variant: 'error',
      });
      onUpdated();
    }
  }, [isError, enqueueSnackbar, onUpdated]);

  useEffect(() => {
    if (isSuccess) {
      enqueueSnackbar('Успех', {
        variant: 'success',
      });
      onUpdated();
    }
  }, [isSuccess, enqueueSnackbar, onUpdated]);

  return (
    <Box alignItems="center" display="flex" mt={2}>
      <StyledIconButton disabled={disabled} loading={isLoading} onClick={clickHandler} variant="outlined">
        <ClearIcon />
      </StyledIconButton>

      {providerDisplayName ?? data.loginProvider}
    </Box>
  );
};

export { DeleteMyExternalLogin };
export type { DeleteMyExternalLoginProps };

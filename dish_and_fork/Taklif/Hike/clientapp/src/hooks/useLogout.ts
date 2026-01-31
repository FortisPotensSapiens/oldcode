import { useOidc } from '@axa-fr/react-oidc';
import { useCallback } from 'react';

import { useAuthConfig } from '~/api';
import { useDeleteDevice } from '~/api/v1/useDeleteDevice';
import { useGetMyDevices } from '~/api/v1/useGetMyDevices';
import config from '~/config';

type UseLogout = () => () => void;

const useLogout: UseLogout = () => {
  const { data } = useAuthConfig();
  const { logout } = useOidc();
  const { refetch } = useGetMyDevices(
    {
      pageNumber: 1,
      pageSize: 999,
    },
    false,
  );
  const deleteDevice = useDeleteDevice();

  return useCallback(async () => {
    const deviceToken = localStorage.getItem(config.messaging.localStorageToken);

    const fetched = await refetch({});

    const item = fetched.data?.items?.find((item) => {
      return item.fcmPushToken === deviceToken;
    });

    if (item && deviceToken) {
      await deleteDevice.mutateAsync(item.id);
    }

    logout(data?.post_logout_redirect_uri);
  }, [refetch, logout, data?.post_logout_redirect_uri, deleteDevice]);
};

export { useLogout };

import { useOidc } from '@axa-fr/react-oidc';
import notification from 'antd/es/notification';
import { onMessage } from 'firebase/messaging';
import { FC, useEffect, useState } from 'react';

import { useGetMyDevices } from '~/api/v1/useGetMyDevices';
import { usePostAddDevice } from '~/api/v1/usePostAddDevice';
import config from '~/config';
import { initFirebase } from '~/notifications';

const FirebaseProvider: FC = ({ children }) => {
  const { isAuthenticated } = useOidc();
  const [inited, setInited] = useState(false);
  const [api, contextHolder] = notification.useNotification();

  const addDeviceMutation = usePostAddDevice();
  const { refetch, data } = useGetMyDevices(
    {
      pageNumber: 1,
      pageSize: 999,
    },
    false,
  );

  useEffect(() => {
    if (isAuthenticated) {
      refetch({});
    }
  }, [isAuthenticated, refetch]);

  useEffect(() => {
    if (data && !inited) {
      initFirebase().then((messaging) => {
        if (messaging) {
          onMessage(messaging, (payload) => {
            api.info({
              message: payload.notification?.title,
              description: payload.notification?.body,
              placement: 'topRight',
            });
          });
        }
      });

      const deviceToken = localStorage.getItem(config.messaging.localStorageToken);

      const isExists = data.items?.find((item) => {
        return item.fcmPushToken === deviceToken;
      });

      if (!isExists && deviceToken) {
        addDeviceMutation.mutateAsync({
          fcmPushToken: deviceToken,
        });

        setInited(true);
      }
    }
  }, [addDeviceMutation, api, data, inited]);

  return (
    <>
      {contextHolder}
      {children}
    </>
  );
};

export { FirebaseProvider };

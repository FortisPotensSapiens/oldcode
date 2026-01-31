import { MouseEventHandler, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';

type UseGoTo = () => MouseEventHandler<HTMLButtonElement>;

const useGoTo: UseGoTo = () => {
  const navigate = useNavigate();

  return useCallback<MouseEventHandler<HTMLButtonElement>>(
    (event) => {
      const href = event.currentTarget.getAttribute('data-href');

      if (href) {
        navigate(href);
      }
    },
    [navigate],
  );
};

export { useGoTo };
export type { UseGoTo };

import { MouseEventHandler, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';

import { getHomepagePath } from '~/routing';

type UseClick = (onClick?: MouseEventHandler<HTMLButtonElement>) => MouseEventHandler<HTMLButtonElement>;

const useClick: UseClick = (onClick) => {
  const navigate = useNavigate();

  return useCallback(
    (event) => {
      if (onClick) {
        onClick(event);
      } else {
        navigate(getHomepagePath());
      }
    },
    [onClick, navigate],
  );
};

export { useClick };

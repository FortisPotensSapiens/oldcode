import { FC, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

import { getHomepagePath } from '~/routing';

const Logout: FC = () => {
  const navigate = useNavigate();

  useEffect(() => {
    navigate(getHomepagePath());
  }, [navigate]);

  return null;
};

export { Logout };

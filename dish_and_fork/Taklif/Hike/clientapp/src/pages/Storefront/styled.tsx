import { Box, Paper, styled } from '@mui/material';
import { Input, Radio, Select } from 'antd';

export const StorefrontContainer = styled(Box)(({ theme }) => ({
  display: 'grid',
  gridTemplateColumns: 'fit-content(200px) 1fr',
  [theme.breakpoints.down('sm')]: {
    gridTemplateColumns: '100%',
  },
}));

export const FiltersContainer = styled(Paper)(({ theme }) => ({
  padding: theme.spacing(2),
  zIndex: '10',
}));

export const StyledItemsContainer = styled('div')`
  padding-left: 1rem;
  padding-top: 1rem;
  padding-right: 1rem;

  @media (max-width: 800px) {
    padding-top: 0;
  }
`;

export const StyledSearch = styled(Input)`
  width: 100%;
  .ant-btn {
    box-shadow: none;
  }
`;

export const StyledSorting = styled(Radio.Group)`
  margin-top: 20px;
  margin-bottom: 10px;
`;

export const StyledSortingSelect = styled(Select)`
  width: 100%;
  margin-top: -5px;
  margin-bottom: 10px;
`;

export const StyledSortingText = styled('span')`
  display: inline-block;
  padding-left: 5px;
  font-size: 80%;
`;

export const StyledButton = styled(Radio.Button)`
  height: 33px !important;
  line-height: 31px !important;
`;

export const GoodsContainer = styled(Box)(() => ({}));

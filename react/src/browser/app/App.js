import * as themes from './themes';
import favicon from '../../common/app/favicon';
import Helmet from 'react-helmet';
import NotFound from '../notfound/NotFoundPage';
import React, { PropTypes as RPT, PureComponent as Component } from 'react';
import Home from '../home/Page';
import { connect } from 'react-redux';
import { Container } from '../app/components';
import { Match, ThemeProvider } from '../../common/app/components';
import { Miss } from 'react-router';

@connect(state => ({
}),
{ })
export default class App extends Component {
  static propTypes = {
  }

  componentDidMount() {

  }

  render() {
    return (
      <ThemeProvider theme={themes.initial}>
        <Container>
          <Helmet
            htmlAttributes={{ lang: 'cs' }}
            meta={[
              { charset: 'utf-8' },
              {
                name: 'viewport',
                content: 'width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no',
              },
              {
                'http-equiv': 'x-ua-compatible',
                content: 'ie=edge',
              },
              {
                name: 'description',
                content: 'MI-VMM',
              },
              {
                name: 'robots',
                content: 'noindex',
              },
              ...favicon.meta,
            ]}
            link={[
              ...favicon.link,
            ]}
          />
          <div style={style.wrapper}>
            <Match exactly pattern="/" component={Home} />
            <Miss component={NotFound} />
          </div>
        </Container>
      </ThemeProvider>
    );
  }
}

const style = {
  wrapper: {
    paddingTop: '50px'
  }
};

import Images from './Images.react';
import React, { PropTypes as RPT, PureComponent as Component } from 'react';
import { connect } from 'react-redux';
import { FileUpload } from 'redux-file-upload';
import { LineChart } from 'react-d3-components';

@connect(state => ({
  dtw: state.api.getIn(['dtw', 'data']),
}))
export default class Dtw extends Component {

  static propTypes = {
    dtw: RPT.array
  }

  render() {
    const { dtw } = this.props;
    console.log(dtw)
    if (!dtw) return null;

    const data = [{
      label: 'name',
      values: dtw.reduce((prev, val) =>
          [...prev, { x: parseInt(val.split(', ')[0], 10), y: parseInt(val.split(', ')[1], 10) }]
        , [])
    }];

    return (
      <LineChart
        data={data}
        width={600}
        height={200}
      />
    );
  }
}

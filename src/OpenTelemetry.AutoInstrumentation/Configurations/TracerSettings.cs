using System;
using System.Collections.Generic;

namespace OpenTelemetry.AutoInstrumentation.Configurations
{
    public class TracerSettings
    {
        // Lazy<T>를 사용한 싱글톤 인스턴스
        private static readonly Lazy<TracerSettings> lazy =
            new Lazy<TracerSettings>(() => new TracerSettings());

        // 싱글톤 인스턴스에 접근하기 위한 public static 프로퍼티
        public static TracerSettings Instance => lazy.Value;

        // 트레이스 활성화 여부
        public bool TracesEnabled { get; set; } = true;

        // 활성화된 계측 목록
        public HashSet<TracerInstrumentation> EnabledInstrumentations { get; private set; }

        // 기본 생성자는 private으로 선언하여 외부에서의 인스턴스화를 방지
        private TracerSettings()
        {
            // 기본값 설정
            TracesEnabled = true; // 기본적으로 트레이스 활성화
            EnabledInstrumentations = new HashSet<TracerInstrumentation>();
            EnabledInstrumentations.Add(TracerInstrumentation.AspNetCore); // 기본적으로 AspNetCore 계측 활성화
        }
    }

    // 사용 가능한 계측 유형을 나열하는 열거형
    public enum TracerInstrumentation
    {
        AspNetCore,
        // 필요한 경우 여기에 다른 계측 유형 추가
    }
}
